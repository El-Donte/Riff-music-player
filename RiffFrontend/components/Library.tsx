'use client'

import useAuthModal from "@/hooks/Modals/useAuthModal";
import useUploadModal from "@/hooks/Modals/useUploadModal";
import MediaItem from "./Items/MediaItem";
import useOnPlay from "@/hooks/useOnPlay";
import Skeleton from "@mui/material/Skeleton";
import Box from "@mui/material/Box";

import { useUser } from "@/hooks/useUser";
import { Track } from "@/types";
import { AiOutlinePlus } from "react-icons/ai";
import { TbPlaylist } from "react-icons/tb";

interface LibraryProps{
    tracks: Track[];
    loading: boolean;
}

const Library: React.FC<LibraryProps> = ({tracks, loading}) =>{
    const authModal = useAuthModal();
    const uploadModal = useUploadModal();
    const { user } = useUser();

    const onPlay = useOnPlay(tracks);

    const upLoad = () =>{
        if(!user){
            authModal.onOpen();
        }

        uploadModal.onOpen();
    };

    return (
        <div className="flex flex-col">
            <div 
            className="
                flex
                items-center
                justify-between
                px-5
                py-4
            ">
                <div
                className="
                    inline-flex
                    items-center
                    gap-x-2
                ">
                    <TbPlaylist size={26} className="text-neutral-400"/>
                    <p className="
                        text-neutral-400
                        text-medium
                        text-md
                    "
                    >
                      Моя медиатека
                    </p>
                </div>
                <AiOutlinePlus
                    onClick={upLoad}
                    size={26}
                    className="
                        text-neutral-400
                        cursor-pointer
                        hover:text-white
                        transition
                    "
                />
            </div>
            <div className="
                flex
                flex-col
                gap-y-2
                mt-4
                px-3
            ">
                {(loading ? Array.from(new Array(5)) : tracks).map((item, index) => (
                    <Box key={index}>
                        {item ? (
                            <MediaItem
                                onClick={(id:string) => onPlay(id)}
                                key={item.id}
                                track={item}
                            />
                        ) : (
                            <Box
                                sx={{
                                    display: "flex",
                                    flexDirection: "row",
                                    alignItems: "center",
                                    gap: 3,
                                    width: "100%",
                                    p: 1,
                                    borderRadius: 3,
                                    bgcolor: "neutral.900",
                                    transition: "all 0.2s",
                                }}
                            >
                                <Skeleton
                                    variant="rectangular"
                                    sx={{
                                    width: { xs: 48, sm: 56, md: 64 },
                                    height: { xs: 48, sm: 56, md: 64 },
                                    borderRadius: 2,
                                    flexShrink: 0,
                                    bgcolor: "rgb(230 104 243 / 16%)",
                                    }}
                                    animation="wave"
                                />

                                <Box sx={{ flexGrow: 1, minWidth: 0 }}>
                                    <Skeleton
                                        variant="text"
                                        sx={{ fontSize: "1.1rem", bgcolor: "rgba(139, 92, 246, 0.12)" }}
                                        width="85%"
                                    />
                                    <Skeleton
                                        variant="text"
                                        sx={{ fontSize: "0.9rem", bgcolor: "rgba(139, 92, 246, 0.08)", mt: 0.8 }}
                                        width="65%"
                                    />
                                </Box>
                            </Box>
                        )} 
                    </Box>
                ))}
            </div>
        </div>
    );
}

export default Library