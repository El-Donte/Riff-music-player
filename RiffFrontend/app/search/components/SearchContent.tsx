'use client';

import LikeButton from "@/components/Basic/LikeButton";
import MediaItem from "@/components/Items/MediaItem";
import useOnPlay from "@/hooks/useOnPlay";
import Box from "@mui/material/Box";
import Skeleton from "@mui/material/Skeleton";

import { Track } from "@/types";

interface SearchContentProps{
    tracks: Track[];
    loading: boolean;
}

const SearchContent: React.FC<SearchContentProps> = ({tracks, loading}) => {
    const onPlay = useOnPlay(tracks);

    if(tracks.length === 0 && !loading){
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
            {(loading ? Array.from(new Array(8)) : tracks).map((track, index) => (
                <Box key={index} >
                    {track ? (
                    <div className="flex items-center gap-x-4 w-full">
                        <div className="flex-1">
                            <MediaItem
                                onClick={(id:string) => onPlay(id)}
                                key={track.id}
                                track={track}
                            />
                        </div>
                        <LikeButton trackId={track.id}/>
                    </div>
                    ) : (
                        <Skeleton animation="wave" variant="rectangular" width={1500} height={63} sx={{ bgcolor: 'rgb(230 104 243 / 16%)' }}/>
                    )} 
                </Box>
            ))}
        </div>
    );
}

export default SearchContent;