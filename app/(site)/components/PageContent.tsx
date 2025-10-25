'use client';

import TrackItem from "@/components/TrackItem";
import useOnPlay from "@/hooks/useOnPlay";
import { Track } from "@/types";

interface PageContentProps {
    tracks: Track[];
}


const PageContent: React.FC<PageContentProps> = ({tracks}) => {
    const onPlay = useOnPlay(tracks);

    if(tracks.length === 0){
        return(
            <div className="mt-4 text-neutral-400">
                Нет доступных песен.
            </div>
        )
    }
    return (
        <div
            className="
                grid
                grid-cols-2
                sm:grid-cols-3
                md:grid-cols-3
                lg:gird-cols-4
                xl:grid-cols-5
                2xl:grid-cols-8
                gap-4
                mt-4
            "
        >
           {tracks.map((item) => (
            <TrackItem
                key={item.id}
                onClick={(id: string) => onPlay(id)}
                track={item}
            />
           ))}
        </div>
    );
}

export default PageContent;