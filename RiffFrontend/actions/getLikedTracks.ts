import { Envelope, Track } from '@/types';
import { cookies } from "next/headers";
import getUser from './getUser';
 
const getLikedTracks = async (): Promise<Track[]> => {
    const user = await getUser();
    const cookiesStore = await cookies();
    
    if(user == null){
        console.log("error");
        return [];
    }

    var response = await fetch(`http://localhost:8080/api/track/like/${user?.id}`, {
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

    if(!envelope.result){
        return [];
    }

    return (envelope.result as any) || [];
}

export default getLikedTracks;