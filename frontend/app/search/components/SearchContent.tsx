'use client';

import LikeButton from "@/components/LikeButton";
import MediaItem from "@/components/MediaItem";
import useOnPlay from "@/hooks/useOnPlay";
import { Track } from "@/types";

interface SearchContentProps{
    tracks: Track[];
}

const SearchContent: React.FC<SearchContentProps> = ({tracks}) => {
    const onPlay = useOnPlay(tracks);

    if(tracks.length === 0){
        return (
            <div
                className="
                    flex
                    flex-col
                    gap-y-2
                    w-full
                    px-6
                    text-neutral-400
                "
            >
                Песни не найдены
            </div>
        )
    }

    return (
        <div className="flex flex-col gap-y-2 w-full px-6">
            {tracks.map((track) => (
                <div
                    key={track.id}
                    className="flex items-center gap-x-4 w-full"
                >
                    <div className="flex-1">
                        <MediaItem
                            onClick={(id:string) => onPlay(id)}
                            track={track}
                        />
                    </div>
                    <LikeButton trackId={track.id}/>
                </div>
            ))}
        </div>
    );
}

export default SearchContent;