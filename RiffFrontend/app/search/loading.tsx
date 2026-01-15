import Header from "@/components/Header";

export default function Loading() {
  return (
    <div
      className="
        bg-background
        rounded-lg
        h-full
        w-full
        overflow-hidden
        overflow-y-auto
      "
    >
      <Header>
        <div className="mb-2 flex flex-col gap-y-6">
          <h1 className="text-white text-3xl font-semibold">
            Поиск
          </h1>
        </div>
      </Header>
    </div>
  );
}