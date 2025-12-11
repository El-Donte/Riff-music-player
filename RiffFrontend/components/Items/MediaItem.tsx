'use client';

import usePlayer from "@/hooks/usePlayer";
import Image from "next/image";

import { Track } from "@/types";
import { Box, Typography } from "@mui/material";

interface MediaItemProps{
    track: Track;
    onClick?: (id:string) => void;
}

const MediaItem: React.FC<MediaItemProps> = ({track, onClick}) =>{
    const player = usePlayer();

    const handleClick = () =>{
        if(onClick){
            return onClick(track.id);
        }

        return player.setId(track.id);
    };

    return (
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
                "&:hover": { bgcolor: "rgba(139, 92, 246, 0.1)" },
            }}
            onClick={handleClick}
        >
                <Box
                    sx={{
                        position: "relative",
                        width: { xs: 48, sm: 56, md: 64 },
                        height: { xs: 48, sm: 56, md: 64 },
                        borderRadius: 2,
                        overflow: "hidden",
                        flexShrink: 0,
                    }}
                >
                    <Image
                        fill
                        src={track.imagePath || '/images/liked.png'}
                        alt="Media Item"
                        className="object-cover transition-transform duration-300 hover:scale-110"
                    />
                </Box>
                <Box sx={{ flexGrow: 1, minWidth: 0 }}>
                    <Typography noWrap fontWeight={600} color="white">
                        {track.title}
                    </Typography>
                    <Typography noWrap color="neutral.400" fontSize="0.9rem">
                        {track.author}
                    </Typography>
                </Box>
        </Box>
    )
}

export default MediaItem;