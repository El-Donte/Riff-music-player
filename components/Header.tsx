"use client";

import { useRouter } from "next/navigation";
import { BiSearch } from "react-icons/bi";
import { HiHome } from "react-icons/hi";
import { RxCaretLeft, RxCaretRight } from "react-icons/rx";
import { twMerge } from "tailwind-merge";
import { FaUserAlt } from "react-icons/fa";
import { toast } from 'react-hot-toast';

import Button from "./Button";
import useAuthModal from "@/hooks/useAuthModal";

interface HeaderProps{
    children: React.ReactNode
    className?: string
}

const Header: React.FC<HeaderProps> = ({children, className}) => {
    const authModal = useAuthModal();
    const router = useRouter();

    const user = true;

    const handleLogOut = () =>{
       router.refresh();

       if(!user){
            toast.error("dadada");
       }else{
        toast.success("Вы вышли из аккаунта")
       }
    }

    return (
        <div className={twMerge(`
            h-fit
            bg-gradient-to-b
            from-emerald-800
            p-6
        `,
        className
        )}>
            
            <div className="
                w-full
                mb-4
                flex
                items-center
                justify-between
            ">
                <div className="
                hidden 
                md:flex 
                gap-x-2 
                items-center">
                    <button 
                    onClick={() => router.back}
                    className="
                        rounded-full
                        bg-black
                        flex
                        items-center
                        justify-center
                        hover:opacity-75
                        transition
                    ">
                        <RxCaretLeft size={35} className="text-white"/>
                    </button>
                    <button 
                    onClick={() => router.forward}
                    className="
                        rounded-full
                        bg-black
                        flex
                        items-center
                        justify-center
                        hover:opacity-75
                        transition
                    ">
                        <RxCaretRight size={35} className="text-white"/>
                    </button>
                </div>
                <div className="
                    flex 
                    md:hidden
                    gap-x-2
                    items-center
                ">
                    <button className="
                        rounded-full
                        p-2
                        bg-white
                        flex
                        items-center
                        hover:opacity-75
                        transition
                    ">
                        <HiHome className="text-black" size={20}/>
                    </button>

                    <button className="
                        rounded-full
                        p-2
                        bg-white
                        flex
                        items-center
                        hover:opacity-75
                        transition
                    ">
                        <BiSearch className="text-black" size={20}/>
                    </button>
                </div>
                <div
                className="
                    flex
                    justify-between
                    items-center
                    gap-x-4
                ">
                    {user ? (
                        <div
                            className="flex gap-x-3 items-center"
                        >
                            <Button
                                onClick={()=>router.push("/account")}
                                className="bg-white"
                            >
                               <FaUserAlt/>
                            </Button>
                            <Button
                                onClick={handleLogOut}
                                className="bg-white px-6 py-2"
                            >
                                Выйти
                            </Button>
                        </div>
                    ): (
                        <>
                            <div>
                            <Button
                                onClick={authModal.onOpen}
                                className="
                                    bg-transparent
                                    text-neutral-300
                                    font-medium
                                "
                                >
                                    Зарегестрироваться
                            </Button>
                            </div>
                            <div>
                            <Button
                                onClick={authModal.onOpen}
                                className="
                                    bg-white
                                    px-6
                                    py-2
                                "
                                >
                                    Войти
                            </Button>
                            </div>
                        </>
                    )}
                </div>
            </div>
            {children}
        </div>
    );
}

export default Header