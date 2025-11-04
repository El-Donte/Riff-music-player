'use client'

import useAuthModal from "@/hooks/useAuthModal";
import useUploadModal from "@/hooks/useUploadModal";
import { useUser } from "@/hooks/useUser";
import { Track } from "@/types";
import { AiOutlinePlus } from "react-icons/ai";
import { TbPlaylist } from "react-icons/tb";
import MediaItem from "./MediaItem";
import useOnPlay from "@/hooks/useOnPlay";

interface LibraryProps{
    tracks: Track[];
}

const Library: React.FC<LibraryProps> = ({tracks}) =>{
    const authModal = useAuthModal();
    const uploadModal = useUploadModal();
    const { user } = useUser();

    const onPlay = useOnPlay(tracks);

    const upLoad = () =>{
        if(!user){
            authModal.onOpen();
        }

        uploadModal.onOpen();
    };

    return (
        <div className="flex flex-col">
            <div 
            className="
                flex
                items-center
                justify-between
                px-5
                py-4
            ">
                <div
                className="
                inline-flex
                items-center
                gap-x-2
                ">
                    <TbPlaylist size={26} className="text-neutral-400"/>
                    <p className="
                        text-neutral-400
                        text-medium
                        text-md
                    "
                    >
                      Моя медиатека
                    </p>
                </div>
                <AiOutlinePlus
                    onClick={upLoad}
                    size={26}
                    className="
                        text-neutral-400
                        cursor-pointer
                        hover:text-white
                        transition
                    "
                />
            </div>
            <div className="
                flex
                flex-col
                gap-y-2
                mt-4
                px-3
            ">
                {tracks.map((item) => (
                    <MediaItem
                        onClick={(id:string) => onPlay(id)}
                        key={item.id}
                        track={item}
                    />
                ))}
            </div>
        </div>
    );
}

export default Library