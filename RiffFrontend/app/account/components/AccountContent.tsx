"use client";

import { useUser } from "@/hooks/useUser";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";

const AccountContent = () => {
    const router = useRouter();
    const {user} = useUser();
    const name = user?.name;
    const [loading, setLoading] = useState();
    useEffect(() => {
        if(!user){
            router.replace('/');
        }
    }, [user, router]);

    return (
        <div className="mb-7 px-6">
            <h2>{name}</h2>
        </div>
    );
};

export default AccountContent;