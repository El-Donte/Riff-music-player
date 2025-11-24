import { Envelope, Track } from "@/types";
import { useEffect, useState, useMemo } from "react";
import toast from "react-hot-toast";

const useGetTrackById = (id?: string) => {
    const [isLoading, setIsLoading] = useState(false);
    const [track, setTrack] = useState<Track | undefined>(undefined)

    useEffect(() => {
        if(!id){
            return;
        }

        setIsLoading(true);

        const fetchSong = async () => {
            const response = await fetch(`http://localhost:8080/api/track/${id}`, {
                credentials: "include"
            });
            
            console.log(response)
            const envelope = (await response.json()) as Envelope<Track>;
            
            if (envelope.errors && envelope.errors.length > 0) {
                setIsLoading(false);
                return toast.error(envelope.errors[0].message);
            }
            
            setTrack(envelope.result as Track)
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