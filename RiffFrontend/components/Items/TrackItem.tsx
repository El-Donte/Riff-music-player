'use client'

import Image from "next/image";
import PlayButton from "../MusicPlayer/PlayButton";

import { Track } from "@/types";

interface TrackItemProps {
    track: Track;
    onClick: (id: string) => void;
}

const TrackItem: React.FC<TrackItemProps> = ({track, onClick}) => {
    return (
        <div
            onClick={() => onClick(track.id)}
            className="
                relative
                group
                flex
                flex-col
                items-center
                justify-center
                rounded-md
                overflow-hidden
                gap-x-4
                bg-dark-violet-900
                cursor-pointer
                hover:bg-electric-violet-400/40
                transition-all
                p-3
                duration-300 group
            "
        >
            <div
                className="
                    relative 
                    aspect-square
                    w-full
                    h-full
                    rounded-md
                    overflow-hidden
                    shrink-0
                    transition-transform duration-300 
                    group-hover:scale-110
                "
            >
                <Image 
                    className="object-cover"
                    src={track.imagePath || "/images/liked.png"}
                    fill
                    alt="Image"
                />
            </div>
            <div
                className="
                    flex
                    flex-col
                    items-start
                    w-full
                    pt-4
                    gap-y-1
                "
            >
                <p className="font-semibold truncate w-full text-zinc-300">
                    {track.title}
                </p>
                <p className="
                    text-neutral-300
                    text-sm
                    pb-4
                    w-full
                    truncate
                ">
                    {track.author}
                </p>
            </div>
            <div className="
                absolute
                bottom-24
                right-5
            ">
                <PlayButton/>
            </div>
        </div>
    );
}

export default TrackItem;