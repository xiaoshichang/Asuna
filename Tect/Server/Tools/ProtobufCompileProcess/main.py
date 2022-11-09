import argparse

protoc_executable_path = "protoc-21.9-win64/bin/protoc.exe"

parse = argparse.ArgumentParser(description="a process to compile protobuf")
parse.add_argument("--side", required=True, choices=["both", "server", "client"])
args = parse.parse_args()


def protobuf_compile_server_side():
    print("start protobuf_compile_server_side")
    


    print("end protobuf_compile_server_side")


def protobuf_compile_client_side():
    print("start protobuf_compile_client_side")
    print("end protobuf_compile_client_side")


if args.side == "server":
    protobuf_compile_server_side()
elif args.side == "client":
    protobuf_compile_client_side()
elif args.side == "both":
    protobuf_compile_server_side()
    protobuf_compile_client_side()
else:
    raise Exception("unknown side")
