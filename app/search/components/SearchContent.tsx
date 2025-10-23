'use client';

import LikedButton from "@/components/LikeButton";
import MediaItem from "@/components/MediaItem";
import { Track } from "@/types";

interface SearchContentProps{
    tracks: Track[];
}

const SearchContent: React.FC<SearchContentProps> = ({tracks}) => {
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
                            onClick={() => {}}
                            track={track}
                        />
                    </div>
                    <LikedButton trackId={track.id}/>
                </div>
            ))}
        </div>
    );
}

export default SearchContent;