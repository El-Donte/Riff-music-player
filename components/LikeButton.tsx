'use client';

import useAuthModal from "@/hooks/useAuthModal";
import { useUser } from "@/hooks/useUser";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { AiFillHeart, AiOutlineHeart } from "react-icons/ai";

interface LikeButtonProps {
    trackId: string;
}

const LikeButton: React.FC<LikeButtonProps> = ({trackId}) => {
    const router = useRouter();

    const authModal = useAuthModal();
    const {user} = useUser();

    const [isLiked, setIsLiked] = useState(false);

    useEffect(() => {
        if(!user){
            return;
        }

        const likedTracks = async () => {
            //TO-DO: получать понравивишиеся
            setIsLiked(true);
        }
    },[trackId])

    const Icon = isLiked ? AiFillHeart : AiOutlineHeart;

    const handleLike = async () => {
        if(!user){
            return authModal.onOpen();
        }

        if(isLiked){
            //TO-DO: удалить из понравившихся
        }else{
            //TO-DO: дабвить в понравившиеся
        }

        router.refresh();
    }

    return (
        <button
        onClick={handleLike}
            className="
                hover:opacity-75
                transition
            "
        >
            <Icon color={isLiked ? '#22c55e' : 'white'} size={25}/>
        </button>
    );
}

export default LikeButton;