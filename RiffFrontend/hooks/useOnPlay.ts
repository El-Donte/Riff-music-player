import { Track } from "@/types";
import { useUser } from "./useUser";

import useAuthModal from "./Modals/useAuthModal";
import usePlayer from "./usePlayer";

const useOnPlay = (tracks: Track[]) => {
    const player = usePlayer();
    const authModal =  useAuthModal();
    const { user } = useUser(); 

    const onPlay = (id: string) => {
        if(!user){
            return authModal.onOpen();
        }

        player.setId(id);
        player.setIds(tracks.map((track) => track.id))
    };

    return onPlay;
};

export default useOnPlay;