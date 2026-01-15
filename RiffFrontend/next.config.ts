import type { NextConfig } from "next";

const nextConfig: NextConfig = {
    images: {
        remotePatterns: [
            {
                protocol: "https",
                hostname: "riff-backet.s3.yandexcloud.net",
                port: "",
                pathname: "/images/**",
            },
            {
                protocol: "https",
                hostname: "riff-backet.storage.yandexcloud.net",
                port: "",
                pathname: "/images/**",
            }
           
        ]
    }
};

export default nextConfig;
