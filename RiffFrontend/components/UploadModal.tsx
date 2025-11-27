"use client";

import { FieldValues, SubmitHandler, useForm } from "react-hook-form";
import { useState } from "react";
import { useRouter } from "next/navigation";


import useUploadModal from "@/hooks/useUploadModal";

import Modal from "./Modal";
import Input from "./Input";
import Button from "./Button";
import toast from "react-hot-toast";
import { useUser } from "@/hooks/useUser";
import { Envelope } from "@/types";
 

const UploadModal = () => {
    const [isLoading, setIsLoading] = useState(false);
    const uploadModal = useUploadModal();
    const router = useRouter();
    const { user } = useUser();

    const {register, handleSubmit, reset} = useForm<FieldValues>({
        defaultValues: {
            author: '',
            title: '',
            track: null,
            image: null,
        }
    });

    const onChange = (open: boolean) => {
        if(!open){
            reset();
            uploadModal.onClose();
        }
    }

    const onSubmit: SubmitHandler<FieldValues> = async (values) => {
        try {
            setIsLoading(true);

            const imageFile = values.image?.[0];
            const trackFile = values.track?.[0];

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

            if (!response.ok || envelope.errors?.length) {
                throw new Error(envelope.errors?.[0]?.message || "Ошибка загрузки");
            }

            toast.success("Песня успешно загружена!");
            reset();
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
                description ="Загрузите mp3 файл"
                isOpen={uploadModal.isOpen}
                onChange={onChange}
            >
                <form
                    onSubmit={handleSubmit(onSubmit)}
                    className="flex flex-col gap-y-4"
                >
                    <Input
                        id="title"
                        disabled={isLoading}
                        {...register('title', { required: true})}
                        placeholder="Название песни"
                    />
                    <Input
                        id="author"
                        disabled={isLoading}
                        {...register('author', { required: true})}
                        placeholder="Автор песни"
                    />
                    <div>
                        <div className="pb-1">
                            Выберите файл песни
                        </div>
                        <Input
                            id="Track"
                            type="file"
                            disabled={isLoading}
                            accept=".mp3"
                            {...register('track', {
                                required: true
                            })}
                        />
                    </div>
                     <div>
                        <div className="pb-1">
                            Выберите файл обложки
                        </div>
                        <Input
                            id="image"
                            type="file"
                            disabled={isLoading}
                            accept="image/a"
                            {...register('image', {
                                required: true
                            })}
                        />
                    </div>
                    <Button disabled={isLoading} type="submit">
                        Загрузить
                    </Button>
                </form>
            </Modal>
        </div>
    );
}

export default UploadModal