'use client'

import useLoadImage from "@/hooks/useLoadImage";
import { Track } from "@/types";
import Image from "next/image";
import PlayButton from "./PlayButton";

interface TrackItemProps {
    track: Track;
    onClick: (id: string) => void;
}

const SongItem: React.FC<TrackItemProps> = ({track, onClick}) => {
    const imagePath = useLoadImage(track);

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
                bg-neutral-400/5
                cursor-pointer
                hover:bg-neutral-400/10
                transition
                p-3
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
                "
            >
                <Image 
                    className="object-cover"
                    src={imagePath || "/images/liked.png"}
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
                <p className="font-semibold truncate w-full">
                    {track.title}
                </p>
                <p className="
                    text-neutral-400
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

export default SongItem;