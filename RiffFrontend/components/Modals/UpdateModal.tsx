"use client";

import useUpdateModal from "@/hooks/Modals/useUpdateModal";
import Modal from "./Modal";
import Input from "../Basic/Input";
import Button from "../Basic/Button";
import toast from "react-hot-toast";

import { FieldValues, SubmitHandler, useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { BiMusic, BiImageAdd } from 'react-icons/bi';
import { useUser } from "@/hooks/useUser";
import { Envelope } from "@/types";
import { UploadDropzone } from "../Basic/Dropzone";
import useGetTrackById from "@/hooks/useGetTrackById";
 

const UpdateModal = () => {
    const [isLoading, setIsLoading] = useState(false);
    const [imagePreview, setImagePreview] = useState<string | undefined>();
    const [trackPath, setTrackPath] = useState<string | undefined>();

    const updateModal = useUpdateModal();
    const router = useRouter();
    const { user } = useUser();
    const { track } = useGetTrackById(updateModal.trackId);

    const {register, setValue, watch, handleSubmit, reset} = useForm<FieldValues>({
        defaultValues: {
            author: track?.author,
            title: track?.title,
            track: null,
            image: null,
        }
    });

    useEffect(() => {
        if (track) {
            setValue("title", track.title);
            setValue("author", track.author);

            if (track.imagePath) {
                setImagePreview(track.imagePath);
            }
            if(track.trackPath){
                setTrackPath(track.trackPath);
            }
        }
    }, [track, setValue]);

    const trackFile = watch('track');
    const imageFile = watch('image');

    const handleTrackDrop = (file: File) => {
        setValue('track', file, { shouldValidate: true });
    };

    const handleImageDrop = (file: File) => {
        setValue('image', file, { shouldValidate: true });
        setImagePreview(URL.createObjectURL(file));
    };

    const onChange = (open: boolean) => {
        if(!open){
            reset();
            setImagePreview(undefined);
            setTrackPath(undefined);
            updateModal.onClose();
        }
    }

    const onSubmit: SubmitHandler<FieldValues> = async (values) => {
        try {
            setIsLoading(true);

            const imageFile = values.image;
            const trackFile = values.track;

            const formData = new FormData();
            formData.append("Title", values.title);
            formData.append("Author", values.author);
            formData.append("UserId", user!.id);
            formData.append("TrackFile", trackFile);
            formData.append("ImageFile", imageFile);

            const response = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/track/${track?.id}`, {
                method: "PATCH",
                credentials: "include",
                body: formData,
            });

            const envelope = await response.json() as Envelope<string>;

            if (envelope.errors?.length) {
                throw new Error(envelope.errors?.[0]?.message || "Ошибка при обновлении");
            }

            toast.success("Песня успешно обновлена!");
            reset();
            setImagePreview(undefined);
            updateModal.onClose();
            router.refresh();

        } catch (error: any) {
            toast.error(error.message || "Не удалось обновить трек");
        } finally {
            setIsLoading(false);
        }
    };

    return (
    <Modal
      title="Обновить песню"
      description="Измените данные трека"
      isOpen={updateModal.isOpen}
      onChange={onChange}
    >
        <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-y-4">
          <Input
            id="title"
            disabled={isLoading}
            {...register('title', { required: "Название обязательно" })}
            placeholder="Название песни"
            className="cursor-pointer"
          />
          <Input
            id="author"
            disabled={isLoading}
            {...register('author', { required: "Автор обязателен" })}
            placeholder="Автор песни"
            className="cursor-pointer"
          />

          <UploadDropzone
            accept={{ 'audio/*': ['.mp3', '.wav', '.ogg'] }}
            onDrop={handleTrackDrop}
            label="Аудиофайл (опционально)"
            description="Перетащите новый MP3 или оставьте текущий"
            icon={<BiMusic className="w-12 h-12 mx-auto mb-4 text-gray-400" />}
            fileName={trackFile?.name || track?.title}
            type="audio"
            currentFileUrl={trackPath}
            isLoading={isLoading}
          />

          <UploadDropzone
            accept={{ 'image/*': ['.jpg', '.jpeg', '.png', '.webp'] }}
            onDrop={handleImageDrop}
            label="Обложка (опционально)"
            description="Новая обложка или оставьте текущую"
            icon={<BiImageAdd className="w-12 h-12 mx-auto mb-4 text-gray-400" />}
            preview={imagePreview}
            currentFileUrl={track?.imagePath}
            fileName={imageFile?.name}
            isLoading={isLoading}
          />

          <Button disabled={isLoading} type="submit">
            Обновить
          </Button>
        </form>
    </Modal>
  );
}

export default UpdateModal