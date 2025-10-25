"use client";

import useGetTrackById from "@/hooks/useGetTrackById";
import useLoadTrackUrl from "@/hooks/useLoadTrackUrl";
import usePlayer from "@/hooks/usePlayer";
import PlayerContent from "./PlayerContent";

const Player = () => {
    const player = usePlayer();
    const { track } = useGetTrackById(player.activeId);

    const trackUrl = useLoadTrackUrl(track!);

    if(!track || !trackUrl || !player.activeId){
        return null;
    }

    return(
        <div
            className="
                fixed
                bottom-0
                bg-black
                w-full
                py-2
                h-20
                px-4
            "
        >
            <PlayerContent
                key = {trackUrl}
                track = {track}
                trackUrl = {trackUrl}
            />
        </div>    
    );
}

export default Player;