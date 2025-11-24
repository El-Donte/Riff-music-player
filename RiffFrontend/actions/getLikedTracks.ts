import { Track } from '@/types';
import { cookies } from "next/headers";
 
const getLikedTracks = async (): Promise<Track[]> => {
    
    const data: Track[] = [];

    if(!data){
        return [];
    }

    return data.map((item) => ({
        ...item
    }))
}

export default getLikedTracks;