"use client";

import { Track } from "@/types";
import { useUser } from "@/hooks/useUser";

import { useRouter } from "next/navigation";
import { useEffect } from "react";
import MediaItem from "@/components/MediaItem";
import LikedButton from "@/components/LikeButton";

interface LikedContentProps{
    tracks: Track[];
};

const LikedContent: React.FC<LikedContentProps> = ({tracks}) =>{
    const router = useRouter();
    const { isLoading, user} = useUser();

    useEffect(() => {
        if(!isLoading && !user){
            router.replace('/');
        }
    }, [isLoading, user, router]);

    if(tracks.length === 0){
        return(
            <div className="
                flex
                flex-col
                gap-y-2
                w-full
                px-6
                text-neutral-400
            ">
                Нет любимых песен.
            </div>
        )
    }

    return(
        <div className="flex flex-col gap-y-2 w-full p-6">
            {tracks.map((track) => (
                <div
                    key = {track.id}
                    className="flex items-center gap-x-4 w-full"
                >
                    <div className="flex-1">
                        <MediaItem
                            onClick={() => {}}
                            track={track}
                        />
                    </div>
                    <LikedButton trackId={track.id}/>
                </div>
            ))}
        </div>
    );
};

export default LikedContent;