'use client'

import { usePathname } from "next/navigation";
import { useMemo } from "react";
import { BiSearch } from "react-icons/bi";
import { HiHome } from "react-icons/hi";

import Box from "./Box";
import SidebarItem from "./SidebarItem";
import Library from "./Library";
import { Track } from "@/types";
import usePlayer from "@/hooks/usePlayer";
import { twMerge } from "tailwind-merge";

interface SidebarProps {
    children : React.ReactNode
    tracks: Track[];
}

const Sidebar: React.FC<SidebarProps> = ({children, tracks}) =>{
    const pathname = usePathname(); 

    const player = usePlayer();

    const routes = useMemo(() => [
        {
            icon: HiHome,
            label: "Домашняя",
            active: pathname !== '/search',
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
                bg-purple-950
                h-full
                w-[300px]
                p-2
            "
        >
                <Box>
                    <div className="
                        flex
                        flex-col
                        gap-y-4
                        px-5
                        py-5
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
                 <Box classname="overflow-y-auto h-full mb-3">
                    <Library tracks={tracks}/>
                </Box>
            </div>
            <main className="h-full flex-1 overflow-y-auto py-2 bg-purple-950">
                {children}
            </main>
        </div>
    );
}

export default Sidebar;