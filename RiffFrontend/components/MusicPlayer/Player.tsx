"use client";

import useGetTrackById from "@/hooks/useGetTrackById";
import usePlayer from "@/hooks/usePlayer";
import PlayerContent from "./PlayerContent";

const Player = () => {
    const player = usePlayer();
    const { track } = useGetTrackById(player.activeId);

    if(!track || !player.activeId){
        return null;
    }

    return(
        <div
            className="
                fixed
                bottom-0
              bg-purple-950
                w-full
                py-4
                h-25
                px-10
            "
        >
            <PlayerContent
                key = {track.trackPath}
                track = {track}
                trackUrl = {track.trackPath}
            />
        </div>    
    );
}

export default Player;