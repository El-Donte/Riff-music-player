'use client';

import usePlayer from "@/hooks/usePlayer";
import Image from "next/image";

import { Track } from "@/types";

interface MediaItemProps{
    track: Track;
    onClick?: (id:string) => void;
}

const MediaItem: React.FC<MediaItemProps> = ({track, onClick}) =>{
    const player = usePlayer();

    const handleClick = () =>{
        if(onClick){
            return onClick(track.id);
        }

        return player.setId(track.id);
    };

    return (
       <div
            onClick={handleClick}
            className="flex items-center gap-3 p-2"
        >

            <div className="
                relative w-12 h-12 sm:w-14 sm:h-14 
                rounded-lg overflow-hidden shrink-0
                transition-transform duration-300 
                group-hover:scale-110
            ">
                <Image
                    fill
                    src={track.imagePath || "/images/liked.png"}
                    alt={track.title}
                    className="object-cover"
                />
            </div>

            <div className="flex-1 min-w-0">
                <p className="text-zinc-300 font-medium truncate text-sm sm:text-base">
                    {track.title}
                </p>
                <p className="text-neutral-300 text-xs sm:text-sm truncate">
                    {track.author}
                </p>
            </div>
        </div>
    )
}

export default MediaItem;