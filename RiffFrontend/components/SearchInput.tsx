'use client';

import qs from "query-string";
import useDebounce from "@/hooks/useDebounce";
import Input from "./Basic/Input";

import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";

const SearchInput = () => {
    const router = useRouter();
    const [value, setValue] = useState<string>("");
    const debouncedValue = useDebounce<string>(value, 500);

    useEffect(() => {
        const query = {
            title:debouncedValue,
        };

        const url = qs.stringifyUrl({
            url: '/search',
            query: query,
        });

        router.push(url);
    }, [debouncedValue, router]);

    return (
        <Input
            placeholder="Что вы хотите послушать ?"
            value={value}
            onChange={(e) => setValue(e.target.value)}
            className="cursor-pointer"
        />
    );
}

export default SearchInput