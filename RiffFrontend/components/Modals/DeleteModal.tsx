"use client";

import useDeleteModal from "@/hooks/Modals/useDeleteModal";
import Modal from "./Modal";
import toast from "react-hot-toast";
import Button from "../Basic/Button";

import { useRouter } from "next/navigation";
import { useUser } from "@/hooks/useUser";
import { Envelope } from "@/types";

 

const DeleteModal = () => {
    const deleteModal = useDeleteModal();
    const router = useRouter();
    const { user } = useUser();

    const onChange = (open: boolean) => {
        if (!open) {
            deleteModal.onClose();
        }
    };

    const handleDelete = async () => {
        if (!deleteModal.trackId || !user) {
            toast.error("Невозможно удалить трек");
            return;
        }

        try {
            const response = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/track/${deleteModal.trackId}`, {
                method: "DELETE",
                credentials: "include"
            });

            const envelope = (await response.json()) as Envelope<string>;

            if (!response.ok || envelope.errors?.length) {
                throw new Error(envelope.errors?.[0]?.message || "Ошибка при удалении");
            }

            toast.success("Песня успешно удалена!");
            deleteModal.onClose();
            router.refresh();
        } catch (error: any) {
            toast.error(error.message || "Не удалось удалить трек");
        }
    };

  return (
    <Modal
      title="Удалить песню"
      description="Вы точно хотите удалить эту песню? Действие нельзя отменить."
      isOpen={deleteModal.isOpen}
      onChange={onChange}
    >
      <div className="flex justify-end gap-x-3 mt-4">
        <Button
          onClick={handleDelete}
          className="bg-red-600 hover:bg-red-500 text-white"
        >
          Подтвердить
        </Button>
        <Button
          onClick={deleteModal.onClose}
          className="bg-neutral-700 hover:bg-neutral-600 text-white"
        >
          Отмена
        </Button>
      </div>
    </Modal>
  );
};

export default DeleteModal;