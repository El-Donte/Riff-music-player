'use client';

import UploadModal from "@/components/Modals/UploadModal";
import AuthModal from "@/components/Modals/AuthModal";
import RegisterModal from "@/components/Modals/RegisterModal";

import { useEffect, useState } from "react";
import UpdateModal from "@/components/Modals/UpdateModal";
import DeleteModal from "@/components/Modals/DeleteModal";

const ModalProvider = () => {
    const [isMounted, setIsMounted] = useState(false);

    useEffect(() => {
        setIsMounted(true);
    }, []);

    if(!isMounted){
        return null;
    }

    return (
         <>
           <AuthModal/>
           <RegisterModal/>
           <UploadModal/>
           <UpdateModal/>
           <DeleteModal/>
         </>

    );
}

export default ModalProvider;