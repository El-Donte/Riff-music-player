'use client'

import { AiOutlinePlus } from "react-icons/ai";
import { TbPlaylist } from "react-icons/tb";

const Library = () =>{
    const upLoad = () =>{
        //To-Do: загрузка
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
                Список Песен
            </div>
        </div>
    );
}

export default Library