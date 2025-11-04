import { Track } from '@/types';
import { cookies } from "next/headers";
 
const getTracksByUserId = async (): Promise<Track[]> => {
    //TO-DO: имплементировать данный метод когда сделаю апи
    const data: Track[] = [
        {
            id: "1",
            user_id: "1",
            title: "Kingslayer",
            author: "Bring me the horizon",
            track_path: "//songs/Bring_Me_The_Horizon_Babymetal_-_Kingslayer_71431801.mp3",
            image_path: "/images/liked.png"
        },
        {
            id: "2",
            user_id: "1",
            title: "Kingslayer",
            author: "Bring me the horizon",
            track_path: "/songs/Bring_Me_The_Horizon_Babymetal_-_Kingslayer_71431801.mp3",
            image_path: "/images/liked.png"
        },
        {
            id: "3",
            user_id: "1",
            title: "Kingslayer",
            author: "Bring me the horizon",
            track_path: "/songs/Bring_Me_The_Horizon_Babymetal_-_Kingslayer_71431801.mp3",
            image_path: "/images/liked.png"
        },
        {
            id: "4",
            user_id: "1",
            title: "Kingslayer",
            author: "Bring me the horizon",
            track_path: "/songs/Bring_Me_The_Horizon_Babymetal_-_Kingslayer_71431801.mp3",
            image_path: '/images/liked.png'
        },
    ]

    return (data as any) || [];
}

export default getTracksByUserId;