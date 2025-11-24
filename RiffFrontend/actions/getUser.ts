import { Envelope, User } from "@/types";
import { cookies } from "next/headers";

const getUser = async (): Promise<User | null> => {
    const cookiesStore = await cookies();

    const response = await fetch("http://localhost:8080/api/user", {
        headers: {
            Cookie: cookiesStore.toString(),
        },
        credentials: "include",
        cache: "no-store",
    });

    const envelope = (await response.json()) as Envelope<User>;

    if (envelope.errors && envelope.errors.length > 0) {
        return null;
    }

    return envelope.result;
};


export default getUser;