"use client";

import useUploadModal from "@/hooks/Modals/useUploadModal";
import Modal from "./Modal";
import Input from "../Basic/Input";
import Button from "../Basic/Button";
import toast from "react-hot-toast";

import { FieldValues, SubmitHandler, useForm } from "react-hook-form";
import { useState } from "react";
import { useRouter } from "next/navigation";
import { BiMusic, BiImageAdd } from 'react-icons/bi';
import { useUser } from "@/hooks/useUser";
import { Envelope } from "@/types";
import { UploadDropzone } from "../Basic/Dropzone";
 

const UploadModal = () => {
    const [isLoading, setIsLoading] = useState(false);
    const uploadModal = useUploadModal();
    const router = useRouter();
    const { user } = useUser();

    const {register, setValue, watch, handleSubmit, reset} = useForm<FieldValues>({
        defaultValues: {
            author: '',
            title: '',
            track: null,
            image: null,
        }
    });

    const trackFile = watch('track');
    const imageFile = watch('image');

    const [imagePreview, setImagePreview] = useState<string | undefined>();

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
            uploadModal.onClose();
        }
    }

    const onSubmit: SubmitHandler<FieldValues> = async (values) => {
        try {
            setIsLoading(true);

            const imageFile = values.image;
            const trackFile = values.track;

            if (!imageFile || !trackFile || !user) {
                toast.error("Заполните все поля и выберите файлы");
                return;
            }

            const formData = new FormData();
            formData.append("Title", values.title);
            formData.append("Author", values.author);
            formData.append("UserId", user.id);
            formData.append("TrackFile", trackFile);
            formData.append("ImageFile", imageFile);

            const response = await fetch("http://localhost:8080/api/track", {
                method: "POST",
                credentials: "include",
                body: formData,
            });

            const envelope = await response.json() as Envelope<string>;

            if (envelope.errors?.length) {
                throw new Error(envelope.errors?.[0]?.message || "Ошибка загрузки");
            }

            toast.success("Песня успешно загружена!");
            reset();
            setImagePreview(undefined);
            uploadModal.onClose();
            router.refresh();

        } catch (error: any) {
            toast.error(error.message || "Не удалось загрузить трек");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div>
            <Modal
                title="Добавить песню"
                description="Загрузите mp3 файл"
                isOpen={uploadModal.isOpen}
                onChange={onChange}
            >
                <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-y-4">
                    <Input
                        id="title"
                        disabled={isLoading}
                        {...register('title', { required: true })}
                        placeholder="Название песни"
                        className="cursor-pointer"
                    />
                    <Input
                        id="author"
                        disabled={isLoading}
                        {...register('author', { required: true })}
                        placeholder="Автор песни"
                        className="cursor-pointer"
                    />

                    <UploadDropzone
                        accept={{ 'audio/*': ['.mp3', '.wav', '.ogg'] }}
                        onDrop={handleTrackDrop}
                        label="Аудиофайл песни"
                        description="Перетащите MP3 сюда"
                        icon={<BiMusic className="w-12 h-12 mx-auto mb-4 text-gray-400" />}
                        fileName={trackFile?.name}
                        isLoading={isLoading}
                    />

                    <UploadDropzone
                        accept={{ 'image/*': ['.jpg', '.jpeg', '.png', '.webp'] }}
                        onDrop={handleImageDrop}
                        label="Обложка песни"
                        description="Перетащите изображение сюда"
                        icon={<BiImageAdd className="w-12 h-12 mx-auto mb-4 text-gray-400" />}
                        preview={imagePreview}
                        fileName={imageFile?.name}
                        isLoading={isLoading}
                    />

                    <input type="hidden" {...register('track', { required: true })} />
                    <input type="hidden" {...register('image', { required: true })} />

                    <Button disabled={isLoading} type="submit">
                        Загрузить
                    </Button>
                </form>
            </Modal>
        </div>
        );
}

export default UploadModal