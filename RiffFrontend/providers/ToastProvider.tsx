"use client";

import { Toaster } from "react-hot-toast";

const ToasterProvider = () =>{
    return (
        <Toaster
            toastOptions={{
                style:{
                    background: "rgb( 13   15  28)",
                    color: '#fff'
                }
            }}
        />
    );
}

export default ToasterProvider