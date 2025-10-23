import { Track } from '@/types';
import { cookies } from "next/headers";
 
const getTracksByUserId = async (): Promise<Track[]> => {
    //TO-DO: имплементировать данный метод когда сделаю апи
    const data = null;

    return (data as any) || [];
}

export default getTracksByUserId;