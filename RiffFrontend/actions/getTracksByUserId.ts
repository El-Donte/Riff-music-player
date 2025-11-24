import { Envelope, Track } from '@/types';
import getUser from './getUser';
import { cookies } from 'next/headers';
 
const getTracksByUserId = async (): Promise<Track[]> => {
    const user = await getUser();
    const cookiesStore = await cookies();
    
    if(user == null){
        console.log("error");
        return [];
    }

    var response = await fetch(`http://localhost:8080/api/tracks/${user?.id}`, {
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

export default getTracksByUserId;