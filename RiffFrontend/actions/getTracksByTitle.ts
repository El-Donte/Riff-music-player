import { Envelope, Track } from '@/types';
import { cookies } from "next/headers";
import getTracks from './getTracks';
 
const getTracksByTitle = async (title:string): Promise<Track[]> => {

    if(!title){
        const allTracks = await getTracks();
        return allTracks;
    }

    const cookiesStore = await cookies();
    
    var response = await fetch(`http://localhost:8080/api/tracks/${title}`, {
        headers: {
            Cookie: cookiesStore.toString(),
        },
        credentials: "include",
        cache: "no-store",
    });

    const envelope = (await response.json()) as Envelope<Track[]>;

    if (envelope.errors && envelope.errors.length > 0) {
        console.error("API Errors:", envelope.errors);
        return [];
    }

    return (envelope.result as any) || [];
}

export default getTracksByTitle;