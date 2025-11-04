import { Track } from "@/types";

const useLoadImage = (track: Track) => {
    if(!track){
        return null;
    }

    //TO-DO: сделать загрузку картинки


    return track.image_path;
}

export default useLoadImage;