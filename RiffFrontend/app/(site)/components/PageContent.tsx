'use client';

import TrackItem from "@/components/Items/TrackItem";
import useOnPlay from "@/hooks/useOnPlay";

import { Track } from "@/types";
import { Box, Skeleton } from "@mui/material";

interface PageContentProps {
    tracks: Track[];
    loading: boolean;
}

const PageContent: React.FC<PageContentProps> = ({tracks, loading}) => {
    const onPlay = useOnPlay(tracks);

    return (
        <div
            className="
                grid
                grid-cols-2
                sm:grid-cols-3
                md:grid-cols-3
                lg:gird-cols-4
                xl:grid-cols-5
                2xl:grid-cols-8
                gap-4
                mt-4
            "
        >
           {(loading ? Array.from(new Array(24)) : tracks).map((item, index) => (
            <Box key={index} sx={{width: "100%"}}>
                {item ? (
                    <TrackItem
                        key={item.id}
                        onClick={(id: string) => onPlay(id)}
                        track={item}
                    />
                ) : (
                    <Box>
                        <Skeleton
                            variant="rectangular"
                            sx={{
                            width: { xs: 64, sm: 64, md: 128 },
                            height: { xs: 64, sm: 64, md: 128 },
                            borderRadius: 2,
                            flexShrink: 0,
                            bgcolor: "rgb(230 104 243 / 16%)",
                            }}
                            animation="wave"
                        />

                        <Box sx={{ mt: 2, px: 1 }}>
                            <Skeleton
                                variant="text"
                                sx={{ fontSize: "1rem", bgcolor: "rgba(139, 92, 246, 0.1)" }}
                                width="80%"
                            />
                            <Skeleton
                                variant="text"
                                sx={{ fontSize: "0.875rem", bgcolor: "rgba(139, 92, 246, 0.08)", mt: 0.5 }}
                                width="60%"
                            />
                        </Box>
                    </Box>
                )} 
            </Box>
           ))}
        </div>
    );
}

export default PageContent;