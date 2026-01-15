"use client";

import { useRouter } from "next/navigation";
import { BiSearch } from "react-icons/bi";
import { HiHome } from "react-icons/hi";
import { RxCaretLeft, RxCaretRight } from "react-icons/rx";
import { twMerge } from "tailwind-merge";
import { FaUserAlt } from "react-icons/fa";
import { toast } from 'react-hot-toast';
import { useUser } from "@/hooks/useUser";

import Button from "./Basic/Button";
import useAuthModal from "@/hooks/Modals/useAuthModal";
import usePlayer from "@/hooks/usePlayer";
import useRegisterModal from "@/hooks/Modals/useRegisterModal";
import { useEffect, useState } from "react";

interface HeaderProps{
    children: React.ReactNode;
    className?: string;
}

const Header: React.FC<HeaderProps> = ({children, className}) => {
    const authModal = useAuthModal();
    const registerModal = useRegisterModal();
    const router = useRouter();
    const player = usePlayer();
    const { user, logout, isLoading } = useUser();

    const [mounted, setMounted] = useState(false);

    useEffect(() => {
        setMounted(true);
    }, []);

    const handleLogOut = () => {
        logout();
        player.reset();
        
        if (!user) {
            toast.error("Ошибка");
        } else {
            toast.success("Вы вышли из аккаунта");
        }
        router.push('/');
        router.refresh();
    };

    if (!mounted || isLoading) {
        return (
            <div className={twMerge(`
                h-fit
                p-6
                bg-profile-header
            `, className)}>
                <div className="w-full mb-4 flex items-center justify-between">
                    <div className="flex justify-between items-center gap-x-4">
                        <div className="flex gap-x-3 items-center">
                            <Button className="bg-primary-50 w-10 h-10 opacity-0" />
                            <Button className="bg-primary-50 px-6 py-2 opacity-0" />
                        </div>
                    </div>
                </div>
                {children}
            </div>
        );
    }

    return (
        <div className={twMerge(`
            h-fit
            p-6
            bg-profile-header
        `, className)}>
            <div className="w-full mb-4 flex items-center justify-between">
                <div className="hidden md:flex gap-x-2 items-center">
                    <button 
                        onClick={() => router.back()}
                        className="rounded-full bg-primary-200 flex items-center justify-center hover:opacity-75 transition"
                    >
                        <RxCaretLeft size={35} className="text-black"/>
                    </button>
                    <button 
                        onClick={() => router.forward()}
                        className="rounded-full bg-primary-200 flex items-center justify-center hover:opacity-75 transition"
                    >
                        <RxCaretRight size={35} className="text-black"/>
                    </button>
                </div>
                <div className="flex md:hidden gap-x-2 items-center">
                    <button 
                        className="rounded-full p-2 bg-primary-200  flex items-center hover:opacity-75 transition"
                        onClick={() => router.push("/")}
                    >
                        <HiHome className="text-black" size={20}/>
                    </button>
                    <button 
                        className="rounded-full p-2 bg-primary-200  flex items-center hover:opacity-75 transition"
                        onClick={() => router.push("/search")}
                    >
                        <BiSearch className="text-black" size={20}/>
                    </button>
                </div>
                <div className="flex justify-between items-center gap-x-4">
                    {user ? (
                        <div className="flex gap-x-3 items-center">
                            <Button
                                onClick={() => router.push("/account")}
                                className="bg-primary-200 "
                            >
                                <FaUserAlt />
                            </Button>
                            <Button
                                onClick={handleLogOut}
                                className="bg-primary-200  px-6 py-2"
                            >
                                Выйти
                            </Button>
                        </div>
                    ) : (
                        <>
                            <Button
                                onClick={registerModal.onOpen}
                                className="bg-transparent text-neutral-300 font-medium"
                            >
                                Зарегистрироваться
                            </Button>
                            <Button
                                onClick={authModal.onOpen}
                                className="bg-primary-200 px-6 py-2"
                            >
                                Войти
                            </Button>
                        </>
                    )}
                </div>
            </div>
            {children}
        </div>
    );
};

export default Header;