import { Track } from '@/types';
import { cookies } from "next/headers";
 
const getLikedTracks = async (): Promise<Track[]> => {
    //TO-DO: имплементировать данный метод когда сделаю апи
    const data: Track[] = [];

    if(!data){
        return [];
    }

    return data.map((item) => ({
        ...item
    }))
}

export default getLikedTracks;