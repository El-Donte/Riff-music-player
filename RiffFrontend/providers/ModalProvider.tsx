'use client';

import { useEffect, useState } from "react";

import UploadModal from "@/components/UploadModal";
import AuthModal from "@/components/AuthModal";
import RegisterModal from "@/components/RegisterModal";

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