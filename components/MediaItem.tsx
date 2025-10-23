'use client';

import useLoadImage from "@/hooks/useLoadImage";
import { Track } from "@/types";
import Image from "next/image";

interface MediaItemProps{
    track: Track
    onClick?: (id:string) => void;
}

const MediaItem: React.FC<MediaItemProps> = ({track, onClick}) =>{
    const imageUrl = useLoadImage(track); 

    const handleClick = () =>{
        if(onClick){
            return onClick(track.id);
        }

        // TO-DO: запуск плеера
    }

    return (
        <div
            onClick={handleClick}
            className="
                flex
                items-center
                gap-x-3
                cursor-pointer
                hover:bg-neutral-800/50
                w-full
                p-2
                rounded-md
            "
        >
            <div
              className="
                relative
                rounded-md
                min-h-[48px]
                min-w-[48px]
                overflow-hidden
              "  
            >
                <Image
                    fill
                    src={imageUrl || '/images/liked.png'}
                    alt="Media Item"
                    className="Object-cover"
                />
            </div>
            <div className="
                flex
                flex-col
                gap-y-1
                overflow-hidden
            ">
                <p className="text-white truncate">
                    {track.title}
                </p>
                <p className="text-neutral-400 text-sm truncate">
                    {track.author}
                </p>
            </div>
        </div>
    )
}

export default MediaItem;