// 'use client';

// import useAuthModal from "@/hooks/useAuthModal";
// import { useUser } from "@/hooks/useUser";
// import { Envelope } from "@/types";
// import { useRouter } from "next/navigation";
// import { useEffect, useState } from "react";
// import toast from "react-hot-toast";
// import { AiFillHeart, AiOutlineHeart } from "react-icons/ai";

// interface LikeButtonProps {
//     trackId: string;
// }

// const LikeButton: React.FC<LikeButtonProps> = ({trackId}) => {
//     const router = useRouter();

//     const authModal = useAuthModal();
//     const { user } = useUser();

//     const [isLiked, setIsLiked] = useState(false);

//     useEffect(() => {
//         if(!user?.id){
//             return;
//         }

//         const likedTrack = async () => {
//             const userId = user?.id;

//             var response = await fetch(`http://localhost:8080/api/track/like`, {
//                 method: "PUT",
//                 credentials: "include",
//                 cache: "no-store",
//                 headers: {
//                     "Content-Type": "application/json",
//                 },
//                 body: JSON.stringify({userId, trackId})
//             });
        
//             const envelope = (await response.json()) as Envelope<boolean>;
            
//             if (envelope.errors && envelope.errors.length > 0) {
//                 console.error("API Errors:", envelope.errors);
//                 setIsLiked(false);
//             }

//             setIsLiked(envelope.result!);
//         }

//         likedTrack();
//         router.refresh()
//     },[user?.id,trackId])

//     const Icon = isLiked ? AiFillHeart : AiOutlineHeart;

//     const handleLike = async () => {
//         if(!user){
//             return authModal.onOpen();
//         }
//         const userId = user?.id

//         if(isLiked){
//             var response = await fetch(`http://localhost:8080/api/track/like`, {
//                 method: "DELETE",
//                 credentials: "include",
//                 cache: "no-store",
//                 headers: {
//                     "Content-Type": "application/json", 
//                 },
//                 body: JSON.stringify({userId, trackId})
//             });
        
//             const envelope = (await response.json()) as Envelope<string>;
//             if (envelope.errors && envelope.errors.length > 0) {
//                 toast.error(envelope.errors.at(0)?.message!);
//             }
//             else{
//                 setIsLiked(false);
//                 toast.success("Удалено из любимых!");
//             }
//         }else{
//             var response = await fetch(`http://localhost:8080/api/track/like`, {
//                 method: "POST",
//                 credentials: "include",
//                 cache: "no-store",
//                 headers: {
//                     "Content-Type": "application/json",
//                 },
//                 body: JSON.stringify({userId, trackId})
//             });

//             const envelope = (await response.json()) as Envelope<string>;
            
//             if (envelope.errors && envelope.errors.length > 0) {
//                 toast.error(envelope.errors.at(0)?.message!);
//             }
//             else{
//                 setIsLiked(true);
//                 toast.success("Добавлено в любимые!");
//             }
//         }

//         router.refresh();
//     }

//     return (
//         <button
//         onClick={handleLike}
//             className="
//                 hover:opacity-75
//                 transition
//             "
//         >
//             <Icon color={isLiked ? '#22c55e' : 'white'} size={25}/>
//         </button>
//     );
// }

// export default LikeButton;


// components/LikeButton.tsx
'use client';


import toast from "react-hot-toast";
import { AiFillHeart, AiOutlineHeart } from "react-icons/ai";
import useAuthModal from "@/hooks/useAuthModal";
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
            <Icon color={isLiked ? '#22c55e' : 'white'} size={25} />
        </button>
    );
};

export default LikeButton;