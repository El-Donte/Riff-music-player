'use client'

import Box from "./Basic/Box";
import SidebarItem from "./Items/SidebarItem";
import Library from "./Library";
import usePlayer from "@/hooks/usePlayer";

import { Track } from "@/types";
import { twMerge } from "tailwind-merge";
import { usePathname } from "next/navigation";
import { useMemo } from "react";
import { BiSearch } from "react-icons/bi";
import { HiHome } from "react-icons/hi";

interface SidebarProps {
    children : React.ReactNode
    tracks: Track[];
    loading: boolean;
}

const Sidebar: React.FC<SidebarProps> = ({children, tracks, loading}) =>{
    const pathname = usePathname(); 

    const player = usePlayer();

    const routes = useMemo(() => [
        {
            icon: HiHome,
            label: "Домашняя",
            active: pathname === '/',
            href: '/',
        },
        {
            icon: BiSearch,
            label: "Поиск",
            active: pathname === '/search',
            href: '/search',
        }
    ], [pathname])

    return(
        <div className={twMerge(`*
            flex
            h-full
        `,
            player.activeId && "h-[calc(100%-80px)]"
        )}>
            <div 
            className="
                hidden 
                md:flex 
                flex-col 
                gap-y-2
                bg-dark-violet-500/40
                h-full
                w-[410px]
                p-4
            "
            >
                <Box>
                    <div className="
                        flex
                        flex-col
                        gap-y-4
                        px-5
                        py-5
                        bg-background
                        rounded-md
                    ">
                        {routes.map((item) =>
                        (
                            <SidebarItem
                                key={item.label}
                                {...item}
                            />
                        ))}
                    </div>
                </Box>
                 <Box classname="overflow-y-auto h-full bg-background">
                    <Library tracks={tracks} loading = {loading}/>
                </Box>
            </div>
            <main className="h-full flex-1 overflow-y-auto py-4 bg-dark-violet-500/40">
                {children}
            </main>
        </div>
    );
}

export default Sidebar;