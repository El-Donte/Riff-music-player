import { Track } from '@/types';
import { cookies } from "next/headers";
import getTracks from './getTracks';
 
const getTracksByTitle = async (title:string): Promise<Track[]> => {
    //TO-DO: имплементировать данный метод когда сделаю апи
    if(!title){
        const allTracks = await getTracks();
        return allTracks;
    }

    const tracks = null;

    return (tracks as any) || [];
}

export default getTracksByTitle;