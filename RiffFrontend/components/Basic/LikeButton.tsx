'use client';

import toast from "react-hot-toast";
import useAuthModal from "@/hooks/Modals/useAuthModal";

import { AiFillHeart, AiOutlineHeart } from "react-icons/ai";
import { useUser } from "@/hooks/useUser";
import { useRouter } from "next/navigation";
import { useLike } from "@/hooks/useLikes";

interface LikeButtonProps {
    trackId: string;
}

const LikeButton: React.FC<LikeButtonProps> = ({ trackId }) => {
    const router = useRouter();
    const authModal = useAuthModal();
    const { user } = useUser();

    const { likedTrackIds, toggleLike } = useLike();
    const isLiked = likedTrackIds.has(trackId);

    const Icon = isLiked ? AiFillHeart : AiOutlineHeart;

    const handleLike = async () => {
        if (!user) return authModal.onOpen();

        toggleLike(trackId);

        const method = isLiked ? "DELETE" : "POST";

        try {
            const res = await fetch(`http://localhost:8080/api/track/like`, {
                method,
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ userId: user.id, trackId }),
            });

            const data = await res.json();

            if (data.errors?.length) {
                toggleLike(trackId);
                toast.error(data.errors[0].message);
            } else {
                toast.success(isLiked ? "Удалено из любимых!" : "Добавлено в любимые!");
                router.refresh();
            }
        } catch {
            toggleLike(trackId);
            toast.error("Ошибка сети");
        }
    };

    return (
        <button onClick={handleLike} className="hover:opacity-75 transition">
            <Icon color={isLiked ? 'rgb(169 139 218)' : 'white'} size={25} className=" hover:text-thistle-300 transition"/>
        </button>
    );
};

export default LikeButton;