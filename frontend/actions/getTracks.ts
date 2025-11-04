import { Track } from '@/types';
import { cookies } from "next/headers";
 
const getTracks = async (): Promise<Track[]> => {
    //TO-DO: имплементировать данный метод когда сделаю апи
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
            track_path: "/public/songs/The Vanished People - SOUL VACATION.mp3",
            image_path: '/images/liked.png'
        },
    ]

    return (data as any) || [];
}

export default getTracks;