import { useEffect } from "react";
import usePlayer from "@/hooks/usePlayer";
import { Track } from "@/types";

export const useMediaSession = (track: Track, play: Function, pause: Function) => {
    const player = usePlayer();

    useEffect(() => {
        if (!("mediaSession" in navigator)) return;

        if (!track?.title || !track?.author) {
            navigator.mediaSession.metadata = null;
            return;
        }

        navigator.mediaSession.setActionHandler("play", () => {
            play();
        });
        navigator.mediaSession.setActionHandler("pause", () => {
            pause();
        });
        navigator.mediaSession.setActionHandler("previoustrack", () => {
            player.previous();
        });
        navigator.mediaSession.setActionHandler("nexttrack", () => {
            player.next();
        });

        return () => {
            navigator.mediaSession.metadata = null;
            navigator.mediaSession.setActionHandler("play", null);
            navigator.mediaSession.setActionHandler("pause", null);
            navigator.mediaSession.setActionHandler("previoustrack", null);
            navigator.mediaSession.setActionHandler("nexttrack", null);
        };
    }, [track, play, pause, player]);
};