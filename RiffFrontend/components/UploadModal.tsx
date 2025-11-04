"use client";

import { FieldValues, SubmitHandler, useForm } from "react-hook-form";
import { useState } from "react";
import { useRouter } from "next/navigation";
import uniqid from "uniqid";

import useUploadModal from "@/hooks/useUploadModal";

import Modal from "./Modal";
import Input from "./Input";
import Button from "./Button";
import toast from "react-hot-toast";
import { useUser } from "@/hooks/useUser";
 

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

    const onSubmit: SubmitHandler<FieldValues> = async (values) =>{
        try{
            setIsLoading(true);

            const imageFile = values.image?.[0];
            const trackFile = values.track?.[0];

            if(!imageFile || ! trackFile || !user){
                toast.error("Отсутствуют значения");
                return;
            }

            const uniqueID = uniqid();

            //TO-DO: обращение к апи и загрузка туда файлов

            router.refresh();
            setIsLoading(false);;
            toast.success('Песня загружена');
            reset();
            uploadModal.onClose();

        }catch (error){
            toast.error("Произошла ошибка");
        } finally{
            setIsLoading(false);
        }
    }

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
                        {...register('title',{ required: true})}
                        placeholder="Название песни"
                    />
                    <Input
                        id="author"
                        disabled={isLoading}
                        {...register('author',{ required: true})}
                        placeholder="Автор песни"
                    />
                    <div>
                        <div className="pb-1">
                            Выберите файл песни
                        </div>
                        <Input
                            id="track"
                            type="file"
                            disabled={isLoading}
                            accept=".mp3"
                            {...register('track',{ required: true})}
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
                            {...register('image',{ required: true})}
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