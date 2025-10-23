import { Track } from "@/types";

const useLoadImage = (track: Track) => {
    if(!track){
        return null;
    }

    //TO-DO: сделать загрузку картинки

    return "image";
}

export default useLoadImage;