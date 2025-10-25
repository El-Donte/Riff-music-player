import { Track } from "@/types";
import { useEffect, useState, useMemo } from "react";

const useGetTrackById = (id?: string) => {
    const [isLoading, setIsLoading] = useState(false);
    const [track, setTrack] = useState<Track | undefined>(undefined)
    
    const data: Track[] = [
        {
            id: "1",
            user_id: "1",
            title: "Kingslayer",
            author: "Bring me the horizon",
            track_path: "/songs/Bring_Me_The_Horizon_Babymetal_-_Kingslayer_71431801.mp3",
            image_path: "/images/liked.png"
        },
        {
            id: "2",
            user_id: "1",
            title: "Hot topic",
            author: "Bbno$",
            track_path: "/songs/Bbno_-_hot_topic_79658515.mp3",
            image_path: "/images/liked.png"
        },
        {
            id: "3",
            user_id: "1",
            title: "Аттестат",
            author: "Бутырка",
            track_path: "/songs/Butyrka_-_Attestat_v_krovi_20138.mp3",
            image_path: "/images/liked.png"
        },
        {
            id: "4",
            user_id: "1",
            title: "Soul vacation",
            author: "The Vanished People",
            track_path: "/songs/The Vanished People - SOUL VACATION.mp3",
            image_path: '/images/liked.png'
        },
    ]


    useEffect(() => {
        if(!id){
            return;
        }

        setIsLoading(true);

        const fetchSong = async () => {
            setTrack(data[Number(id) - 1] as Track)
            setIsLoading(false);
        }

        fetchSong();
    }, [id]);

    return useMemo(() => ({
        isLoading,
        track
    }), [isLoading, track]);
};

export default useGetTrackById;