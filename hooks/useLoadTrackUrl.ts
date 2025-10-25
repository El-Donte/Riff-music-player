import { Track } from "@/types";

const useLoadTrackUrl = (track: Track) => {
    if(!track){
        return '';
    }

        //TO-DO: сделать загрузку трека

    return track.track_path;
}

export default useLoadTrackUrl;