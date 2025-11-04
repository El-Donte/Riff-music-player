'use client';

import { useEffect, useState } from "react";

import Modal from "@/components/Modal";
import UploadModal from "@/components/UploadModal";
import AuthModal from "@/components/AuthModal";

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
           <UploadModal/>
         </>

    );
}

export default ModalProvider;