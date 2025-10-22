'use client';

import { useRouter } from "next/navigation";

import Modal from "./Modal";
import useAuthModal from "@/hooks/useAuthModal";

const AuthModal =() =>{
    const router = useRouter();
    const { onClose, isOpen } = useAuthModal();

    const onChange = (open: boolean) =>{
        if(!open){
            onClose();
        }
    }

    return(
        <Modal
            title="С возвращением"
            description="Войти в свой аккаунт"
            isOpen={isOpen}
            onChange={onChange}
        >
            вфвфвф
        </Modal>
    );

}

export default AuthModal;