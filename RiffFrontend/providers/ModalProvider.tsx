'use client';

import UploadModal from "@/components/Modals/UploadModal";
import AuthModal from "@/components/Modals/AuthModal";
import RegisterModal from "@/components/Modals/RegisterModal";

import { useEffect, useState } from "react";

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
         </>

    );
}

export default ModalProvider;